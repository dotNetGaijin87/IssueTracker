import { createRoot } from 'react-dom/client';
import App from './layout/app/App';
import './index.css';

const container = document.getElementById('root');

if (container === null) {
  throw new Error('Missing #root element: index.html and index.tsx disagree.');
}

createRoot(container).render(<App />);
