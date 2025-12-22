# Sample Manager v2 — Project Plan

## Overview
This document outlines the major goals, features, and technical directions for version 2 of Sample Manager. It is based on lessons learned from v1 and aims to make the project more powerful, maintainable, and user-friendly, especially for large libraries and advanced search/filtering workflows.

---

## Frontend v2 Goals

### 1. Global State Management (Zustand)
- Introduce Zustand for managing global state (filters, search, selection).
- Filters/search state should be accessible and updatable from any component.
- Enables persistent filters, cross-component UI sync, and easier future features.

### 2. Routing for Selection
- Use React Router to encode folder/file/pack selection in the URL.
- Make file/folder/pack elements anchor tags; navigation handled centrally.
- Enables deep linking, browser navigation, and a more robust app structure.

### 3. Advanced Search & Filter UI
- Build a dedicated search/filter panel or overlay.
- Integrate with global state and backend search API.
- Support facets (pack, type, tags, duration, etc.) and free-text search.

---

## Backend v2 Goals

### 1. Sample Pack Info API
- Expose richer endpoints for packs (list, details, files in pack).
- Allow querying packs by attributes (vendor, license, etc.).

### 2. Database Index for Files
- Integrate a database (sqlite or similar) to index all files and metadata.
- Enables fast search/filtering, complex queries, and future features (e.g., favorites, tags).
- DB acts as a cache/index, not the source of truth—sync with filesystem on startup or via watcher.

### 3. FS/DB Sync Strategy
- Decide on sync model: full rescan on startup, or incremental updates (watcher).
- DB should be rebuilt or updated if files change (add/remove/rename).
- Document sync logic and edge cases (e.g., file deleted on disk but present in DB).

### 4. New Services
- File indexing/scanning service (FS → DB).
- Search/query service (DB → API).
- Possibly a background sync/refresh service.

---

## Out of Scope for v2 (v3+ Ideas)
- Transcoding (e.g., auto-convert to .ogg/.mp3 for preview)
- Flucoma/ML-based organization or tagging
- User accounts, favorites, playlists
- Batch operations (move, delete, tag)
- Cloud sync or remote access

---

## Next Steps & Milestones

### Frontend
- [ ] Add Zustand for global state (filters, selection, search)
- [ ] Refactor navigation to use React Router for selection
- [ ] Design and implement advanced search/filter UI

### Backend
- [ ] Choose and integrate a DB (likely sqlite)
- [ ] Implement file indexing and sync logic
- [ ] Expose new search/filter endpoints
- [ ] Expand pack info endpoints

### Cross-cutting
- [ ] Document new architecture (FS/DB sync, state flows)
- [ ] Update copilot-instructions and specs

---

## Notes
- Keep DB as a cache/index, not the source of truth. Filesystem is always authoritative.
- Prioritize simplicity and maintainability in all new code.
- Anything out of scope for v2 but interesting should be noted for v3+ consideration.
