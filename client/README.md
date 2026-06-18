# Issue Tracker — Frontend

React 18 + TypeScript single-page app, built with [Vite](https://vitejs.dev/) and [Material UI](https://mui.com/).

## Scripts

| Command | Description |
|---|---|
| `npm run dev` | Start the dev server on http://localhost:3000 |
| `npm run build` | Type-check (`tsc`) and produce a production build in `build/` |
| `npm run preview` | Preview the production build locally |
| `npm run lint` | Run ESLint |
| `npm run cy:open` | Open Cypress for end-to-end tests |

## Configuration

API and Auth0 settings live in [`src/AppSettings.ts`](src/AppSettings.ts). The app expects the
backend API to be running (see the root [README](../README.md) for instructions).

> **Note:** the project pins React 18-compatible versions. `.npmrc` sets `legacy-peer-deps=true`
> because `@asseinfo/react-kanban` still declares React 16/17 peer ranges, though it works fine on React 18.
