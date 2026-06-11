// API Configuration
// ВАЖЛИВО: API працює на ІНШОМУ домені/порті для демонстрації CORS
const API_CONFIG = {
    BASE_URL: 'https://localhost:7216/api',  // Web API на порту 7216
    TIMEOUT: 10000 // 10 секунд
};

// CORS буде працювати тому що:
// 1. SPA: http://localhost:8080 (або Live Server)
// 2. API: https://localhost:7216
// Це різні домени/порти - потрібен CORS!
