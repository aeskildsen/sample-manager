// Matches Models/FolderNode.cs
export interface FolderNode {
  name: string;
  path: string;
  subFolders: FolderNode[];
}

// Matches Models/FolderContents.cs
export interface FolderContents {
  path: string;
  subFolders: string[];
  audioFiles: AudioFile[];
}

// Matches Models/AudioFile.cs
export interface AudioFile {
  name: string;
  filePath: string;
  fileName: string;
  category: string;
  packSlug: string;
  licenseSlug: string;
  sampleRate?: number;
  duration?: number;
  channels?: number;
  bitDepth?: number;
  bitRate?: number;
  tags?: string;
  format: string;
}