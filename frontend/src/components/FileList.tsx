import './FileList.css'
import type { FolderContents } from "../types/api";
import type { fileSelectionHandler } from '../types/ui';
import { formatTime } from '../utils/durations';
import Chip from './Chip'
import { useState } from 'react'

interface FileListProps {
  folderContents: FolderContents | null,
  loading: boolean,
  handleFileSelection: fileSelectionHandler
}

export default function FileList({
  folderContents,
  loading,
  handleFileSelection
}: FileListProps) {
  const [selectedPack, setSelectedPack] = useState<string | null>(null)
  
  if (loading || !folderContents) {
    return <main className='file-list'></main>
  }

  let files = folderContents.audioFiles.sort((a, b) => a.name.toLowerCase() > b.name.toLowerCase() ? 1 : -1)

  if (selectedPack) {
    files = files.filter(file => file.packSlug == selectedPack)
  }

  const list = files.map(file => {
    return (
    <tr key={file.filePath}>
      <td className="name" onClick={() => handleFileSelection(file)}><span className='name'>{file.name}</span></td>
      <td>{file.format}</td>
      <td className="duration">{formatTime(file.duration)}</td>
      <td>{file.channels}</td>
      <td><Chip clickHandler={() => setSelectedPack(file.packSlug)}>{file.packSlug}</Chip></td>
      <td>{file.licenseSlug}</td>
    </tr>
  )})

  return (
    <main className='file-list panel'>
      <table>
        <thead>
          <tr>
            <th className="name">Name</th>
            <th>Format</th>
            <th className="duration">Duration</th>
            <th>Channels</th>
            <th>Pack</th>
            <th>License</th>
          </tr>
        </thead>
        <tbody>
          {list}
        </tbody>
      </table>
    </main>
  )
}