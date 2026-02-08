import { describe, it, expect, vi, beforeEach } from 'vitest'
import axios from 'axios'

// Mock axios before importing api
const mockPost = vi.fn()
const mockGet = vi.fn()
const mockDelete = vi.fn()

vi.mock('axios', () => ({
  default: {
    create: vi.fn(() => ({
      post: mockPost,
      get: mockGet,
      delete: mockDelete
    }))
  }
}))

// Import api after mocking axios
const { default: api } = await import('../services/api')

describe('API Service', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('analyzeText', () => {
    it('should send text for analysis and return response', async () => {
      const mockContent = 'Hello, how are you?'
      const mockResponse = {
        data: {
          id: 1,
          content: mockContent,
          sentiment: 0,
          sentimentText: 'Positive',
          language: 'English',
          createdAt: new Date().toISOString()
        }
      }

      mockPost.mockResolvedValueOnce(mockResponse)

      const result = await api.analyzeText(mockContent)
      
      expect(result).toEqual(mockResponse.data)
      expect(mockPost).toHaveBeenCalledWith(
        '/TextAnalysis/analyze',
        JSON.stringify(mockContent),
        expect.objectContaining({
          headers: { 'Content-Type': 'application/json' }
        })
      )
    })
  })

  describe('getAllTexts', () => {
    it('should fetch all texts from API', async () => {
      const mockData = [
        { id: 1, content: 'Test 1', language: 'English', sentiment: 0 },
        { id: 2, content: 'Test 2', language: 'Spanish', sentiment: 1 }
      ]

      mockGet.mockResolvedValueOnce({ data: mockData })

      const result = await api.getAllTexts()
      
      expect(result).toEqual(mockData)
      expect(result).toHaveLength(2)
      expect(mockGet).toHaveBeenCalledWith('/TextAnalysis/all')
    })

    it('should handle errors when fetching texts', async () => {
      mockGet.mockRejectedValueOnce(new Error('Network error'))

      await expect(api.getAllTexts()).rejects.toThrow('Network error')
    })
  })

  describe('deleteText', () => {
    it('should delete text by ID', async () => {
      const mockId = 1
      const mockResponse = {
        data: { message: 'Text analysis deleted successfully.', id: mockId }
      }

      mockDelete.mockResolvedValueOnce(mockResponse)

      const result = await api.deleteText(mockId)
      
      expect(result).toEqual(mockResponse.data)
      expect(mockDelete).toHaveBeenCalledWith('/TextAnalysis/1')
    })

    it('should handle errors when deleting text', async () => {
      mockDelete.mockRejectedValueOnce(new Error('Not found'))

      await expect(api.deleteText(999)).rejects.toThrow('Not found')
    })
  })
})
