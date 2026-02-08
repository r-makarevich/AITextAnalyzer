import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import LoginPage from '../views/LoginPage.vue'
import { createRouter, createMemoryHistory } from 'vue-router'

describe('LoginPage', () => {
  let wrapper
  let router

  beforeEach(() => {
    router = createRouter({
      history: createMemoryHistory(),
      routes: [
        { path: '/', name: 'Home', component: { template: '<div>Home</div>' } },
        { path: '/history', name: 'History', component: { template: '<div>History</div>' } },
        { path: '/login', name: 'Login', component: LoginPage }
      ]
    })

    localStorage.clear()
    vi.clearAllMocks()
  })

  it('should render login form', () => {
    wrapper = mount(LoginPage, {
      global: {
        plugins: [router]
      }
    })

    expect(wrapper.find('h4').text()).toContain('Admin Login')
    expect(wrapper.find('input[type="password"]').exists()).toBe(true)
    expect(wrapper.find('button[type="submit"]').exists()).toBe(true)
  })

  it('should show error message for incorrect password', async () => {
    wrapper = mount(LoginPage, {
      global: {
        plugins: [router]
      }
    })

    await wrapper.find('input[type="password"]').setValue('wrongpassword')
    await wrapper.find('form').trigger('submit.prevent')

    // Wait for async operations
    await new Promise(resolve => setTimeout(resolve, 600))

    expect(wrapper.text()).toContain('Invalid password')
  })

  it('should login successfully with correct password', async () => {
    wrapper = mount(LoginPage, {
      global: {
        plugins: [router]
      }
    })

    await wrapper.find('input[type="password"]').setValue('admin123')
    await wrapper.find('form').trigger('submit.prevent')

    // Wait for async operations
    await new Promise(resolve => setTimeout(resolve, 600))

    expect(localStorage.setItem).toHaveBeenCalledWith('isAdmin', 'true')
    expect(wrapper.text()).toContain('Login successful')
  })

  it('should disable form while loading', async () => {
    wrapper = mount(LoginPage, {
      global: {
        plugins: [router]
      }
    })

    await wrapper.find('input[type="password"]').setValue('admin123')
    const form = wrapper.find('form')
    await form.trigger('submit.prevent')

    const submitButton = wrapper.find('button[type="submit"]')
    const passwordInput = wrapper.find('input[type="password"]')

    expect(submitButton.attributes('disabled')).toBeDefined()
    expect(passwordInput.attributes('disabled')).toBeDefined()
  })

  it('should navigate back when back button is clicked', async () => {
    const routerPushSpy = vi.spyOn(router, 'back')
    
    wrapper = mount(LoginPage, {
      global: {
        plugins: [router]
      }
    })

    await wrapper.findAll('button')[1].trigger('click')
    
    expect(routerPushSpy).toHaveBeenCalled()
  })
})
