// API Service - Всі AJAX запити через Fetch API
class ApiService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    // Generic AJAX request
    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}/${endpoint}`;
        
        const defaultOptions = {
            headers: {
                'Content-Type': 'application/json',
            },
            // CORS credentials
            credentials: 'include'
        };

        const config = { ...defaultOptions, ...options };

        try {
            const response = await fetch(url, config);
            
            // Перевірка статусу
            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.error || `HTTP Error: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    // GET request
    async get(endpoint) {
        return this.request(endpoint, {
            method: 'GET'
        });
    }

    // POST request
    async post(endpoint, data) {
        return this.request(endpoint, {
            method: 'POST',
            body: JSON.stringify(data)
        });
    }

    // PUT request
    async put(endpoint, data = null) {
        return this.request(endpoint, {
            method: 'PUT',
            body: data ? JSON.stringify(data) : null
        });
    }

    // DELETE request
    async delete(endpoint) {
        return this.request(endpoint, {
            method: 'DELETE'
        });
    }

    // Users API
    async getUsers() {
        return this.get('users');
    }

    async getUser(id) {
        return this.get(`users/${id}`);
    }

    async toggleUserStatus(id) {
        return this.put(`users/${id}/toggle-status`);
    }

    async deleteUser(id) {
        return this.delete(`users/${id}`);
    }

    async getUserStatistics() {
        return this.get('users/statistics');
    }

    // Songs API
    async getSongs(filters = {}) {
        const params = new URLSearchParams();
        if (filters.title) params.append('title', filters.title);
        if (filters.artist) params.append('artist', filters.artist);
        if (filters.genreId) params.append('genreId', filters.genreId);
        
        const query = params.toString();
        return this.get(`songs${query ? '?' + query : ''}`);
    }

    async getSong(id) {
        return this.get(`songs/${id}`);
    }

    async deleteSong(id) {
        return this.delete(`songs/${id}`);
    }

    async getSongStatistics() {
        return this.get('songs/statistics');
    }

    // Genres API
    async getGenres() {
        return this.get('genres');
    }

    async getGenre(id) {
        return this.get(`genres/${id}`);
    }

    async createGenre(data) {
        return this.post('genres', data);
    }

    async updateGenre(id, data) {
        return this.put(`genres/${id}`, data);
    }

    async deleteGenre(id) {
        return this.delete(`genres/${id}`);
    }

    // Check API health
    async checkHealth() {
        try {
            await this.get('genres');
            return true;
        } catch {
            return false;
        }
    }
}

// Create global API instance
const api = new ApiService(API_CONFIG.BASE_URL);
