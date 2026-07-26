# Issue Tracker — Frontend

React 18 + TypeScript single-page app, built with [Vite](https://vitejs.dev/) and [Material UI](https://mui.com/).

Package manager: **pnpm** (`pnpm-lock.yaml` is the source of truth).

## Scripts

| Command | Description |
|---|---|
| `pnpm dev` | Start the dev server on http://localhost:3000 |
| `pnpm typecheck` | Type-check the project references (`tsc -b`) |
| `pnpm lint` | Type-aware ESLint over the whole project |
| `pnpm format` | Check Prettier formatting |
| `pnpm test` | Run the Vitest suite once |
| `pnpm test:watch` | Run Vitest in watch mode |
| `pnpm test:coverage` | Run the suite with a V8 coverage report |
| `pnpm build` | Type-check and produce a production build in `build/` |
| `pnpm preview` | Preview the production build locally |

## Layout

| Path | Contents |
|---|---|
| `src/models/` | Domain types and the zod schemas that validate API responses |
| `src/adapters/` | `http.ts` transport, `apiError.ts` error normalisation, `adapter.ts` resource facade |
| `src/components/status/` | Generic `StatusBadge` / `StatusSelect` driven by per-domain badge registries |
| `src/features/` | Route-level pages |
| `src/types/` | Ambient declarations for untyped dependencies |

## Type safety

The build runs under `strict` plus `noUncheckedIndexedAccess`, `exactOptionalPropertyTypes`,
`noImplicitReturns`, `noUnusedLocals`/`noUnusedParameters` and `allowJs: false`, and ESLint
runs type-aware rules (`no-floating-promises`, `no-misused-promises`, `no-explicit-any`,
`switch-exhaustiveness-check`, `no-unnecessary-condition`).

API responses are parsed with zod at the boundary in `src/adapters/http.ts`, so the model
types describe data that has actually been checked rather than data that was merely asserted.
Entity ids are branded (`IssueId`, `ProjectId`, `UserId`, `CommentId`) so they cannot be
swapped for one another or for a plain string.

## Configuration

Environment variables are validated at startup in [`src/AppSettings.ts`](src/AppSettings.ts);
copy `.env.example` to `.env` to override the documented dev defaults. The app expects the
backend API to be running (see the root [README](../README.md) for instructions).

> **Note:** `.npmrc` sets `legacy-peer-deps=true` because `@asseinfo/react-kanban` still
> declares React 16/17 peer ranges, though it works fine on React 18. That package ships no
> types; the surface this app uses is declared in `src/types/asseinfo-react-kanban.d.ts`.
