import React, { useRef, useState, useEffect } from 'react'
import { useWavesurfer } from '@wavesurfer/react'
import { FaPlay, FaPause, FaStop } from 'react-icons/fa'
import { ImLoop } from "react-icons/im";
// import styles from './AudioPlayer.module.css'
import './AudioPlayer.css'
import type { AudioFile } from '../types/api'
import { formatTime } from '../utils/durations'

export interface AudioPlayerProps {
  url: string | null
  metadata: AudioFile | null
}

export default function AudioPlayer({
  url,
  metadata,
}: AudioPlayerProps) {
  const containerRef = useRef<HTMLDivElement>(null)
  const [volume, setVolume] = useState(0.7)
  const [isLooping, setIsLooping] = useState(false)
  //const [isMuted, setIsMuted] = useState(false)
  
  const { wavesurfer, isReady, isPlaying, currentTime } = useWavesurfer({
    container: containerRef,
    url: url || '',
    waveColor: 'green',
    progressColor: 'green',
    height: 130,
    barWidth: 2,
    barRadius: 1,
    barGap: 1,
    normalize: true,
    dragToSeek: true,
  })
  
  const duration = wavesurfer?.getDuration() || 0
  
  const handlePlayPause = () => {
    wavesurfer?.playPause();
  }

  const handleStop = () => {
    wavesurfer?.stop();
  }

  const handleVolumeChange = (ev: React.ChangeEvent<HTMLInputElement>) => {
    const newVolume = parseFloat(ev.target.value)
    wavesurfer?.setVolume(newVolume)
    setVolume(newVolume)
  }

  useEffect(() => {
    if (!wavesurfer) return
    const handleFinish = () => {
      if (isLooping) {
        wavesurfer.play()
      }
    }
    const unsubscribe = wavesurfer.on('finish', handleFinish)
    return unsubscribe
  }, [wavesurfer, isLooping])

  return (
    <div className='player panel'>
      
      <div className='controls'>
        <div className='name'>{metadata ? metadata.name : ''}</div>
        <div className='clock'>
          {formatTime(currentTime)} / {formatTime(duration)}
        </div>
        <div className='control-row'>
          <input type="range" min="0" max="1" step="0.01" value={volume} onChange={handleVolumeChange}/>
        </div>
        <div className='control-row'>
          <button onClick={handlePlayPause}>
          { isPlaying ? <FaPause /> : <FaPlay /> }
          </button>
          <button onClick={handleStop}>
            <FaStop />
          </button>
          <button 
            className={isLooping ? 'active' : ''}
            onClick={() => setIsLooping(prev => !prev)}
            >
            <ImLoop />
          </button>
        </div>
      </div>

      <div className='waveform-container'>
        <div ref={containerRef} />
        {(!url || !isReady ) && <p className="placeholder">No file selected.</p>}
      </div>

      <div className='metadata'>
        { metadata && (
        <table>
          <tr><td>Samplerate:</td><td>{metadata.sampleRate ? metadata.sampleRate/1000 : '?'} kHz</td></tr>
          <tr><td>Bit depth:</td><td>{metadata.bitDepth} bit</td></tr>
          <tr><td>Channels:</td><td>{metadata.channels}</td></tr>
        </table>
        )}
      </div>
    </div>
  )
}
