# AI Text Analyzer - Frontend

A Vue.js 3 web application for analyzing text sentiment and detecting language using ML.NET powered backend.

## Features

- **Analyze Page**: Enter text and get instant sentiment analysis and language detection
- **History Page**: View all previous text analyses in a tabular format with details
- **Responsive Design**: Built with Bootstrap 5 for mobile-friendly UI
- **Real-time Results**: Instant feedback with loading states and error handling

## Tech Stack

- **Vue 3**: Progressive JavaScript framework with Composition API
- **Vue Router**: Client-side routing
- **Vite**: Fast build tool and dev server
- **Axios**: HTTP client for API calls
- **Bootstrap 5**: CSS framework for styling
- **Bootstrap Icons**: Icon library

## Prerequisites

- Node.js (v16 or higher)
- npm or yarn
- Backend API running on http://localhost:5196

## Installation

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

## Running the Application

### Development Mode

Start the development server with hot-reload:

```bash
npm run dev
```

The application will be available at http://localhost:3000

### Production Build

Build the application for production:

```bash
npm run build
```

Preview the production build:

```bash
npm run preview
```

## Project Structure

```
frontend/
├── src/
│   ├── views/
│   │   ├── AnalyzePage.vue      # Main text analysis page
│   │   └── HistoryPage.vue      # History view with table
│   ├── services/
│   │   └── api.js               # API service for backend calls
│   ├── router/
│   │   └── index.js             # Vue Router configuration
│   ├── App.vue                  # Root component with navigation
│   └── main.js                  # Application entry point
├── index.html                   # HTML template
├── vite.config.js              # Vite configuration
└── package.json                # Dependencies and scripts
```

## API Integration

The frontend communicates with the backend API at `http://localhost:5196/api/TextAnalysis`:

- **POST /analyze**: Analyze text content
- **GET /all**: Get all text analyses history

## Features in Detail

### Analyze Page
- Text input area for entering content
- Real-time validation
- Loading states during analysis
- Results display with:
  - Detected language with icon
  - Sentiment analysis with color-coded badges
  - Timestamp
  - Full text content

### History Page
- Table view of all analyses
- Sortable by date (newest first)
- Quick view of:
  - ID
  - Truncated text content
  - Language badge
  - Sentiment badge
  - Date and time
- Modal popup for detailed view
- Refresh button to reload data

## Styling

- Uses Bootstrap 5 for responsive layout
- Custom CSS for component-specific styling
- Color-coded sentiment indicators:
  - 🟢 Green for Positive
  - 🔴 Red for Negative
  - ⚪ Gray for Neutral
- Bootstrap Icons for visual elements

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## License

MIT
