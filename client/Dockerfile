# Build the Vite app
FROM node:20-alpine AS builder
WORKDIR /app
# Install dependencies (devDependencies are required for the Vite build)
COPY package.json package-lock.json* .npmrc* ./
RUN npm install
# Copy app files and build
COPY . .
RUN npm run build

# Bundle static assets with nginx
FROM nginx:1.27-alpine AS production
# Copy built assets from builder
COPY --from=builder /app/build /usr/share/nginx/html
# Add your nginx.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Expose port
EXPOSE 80
# Start nginx
CMD ["nginx", "-g", "daemon off;"]
