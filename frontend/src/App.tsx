import { useState } from 'react'
import './App.css'
import Header from './components/Header'
import '@fontsource-variable/inter'
import FolderTree from './components/FolderTree'
import FileList from './components/FileList'
import AudioPlayer from './components/AudioPlayer'
import { useFetch } from './hooks/useFetch'
import type { FolderNode, FolderContents, AudioFile } from './types/api'

const BASE_URL = 'http://localhost:5160/api/'

function App() {
  const [selectedFolder, setSelectedFolder] = useState<string | null>(null)
  const [selectedFile, setSelectedFile] = useState<AudioFile | null>(null)

  const {
    data: folderTree,
    loading: folderTreeLoading,
    error: folderTreeError
  } = useFetch<FolderNode>(BASE_URL + 'foldertree');

  const folderContentsUrl = selectedFolder
    ? BASE_URL + 'folders/' + selectedFolder
    : null

  const {
    data: folderContents,
    loading: folderContentsLoading,
    error: folderContentsError
  } = useFetch<FolderContents>(folderContentsUrl);
  
  return (
    <div className="app-container">
      <Header />
      <FolderTree
        folderTree={folderTree}
        loading={folderTreeLoading}
        handleFolderSelection={setSelectedFolder}
        />
      <nav className='breadcrumbs panel'>Current folder: {selectedFolder}</nav>
      <FileList
        folderContents={folderContents}
        loading={folderContentsLoading}
        handleFileSelection={setSelectedFile}
        />
      <AudioPlayer
        url={selectedFile && BASE_URL + 'files/stream/' + selectedFile.filePath}
        metadata={selectedFile}
        />
    </div>
  )
}

export default App
