import type { AudioFile } from "./api"

export type selectionHandler = (path: string) => void
export type fileSelectionHandler = (file: AudioFile) => void
