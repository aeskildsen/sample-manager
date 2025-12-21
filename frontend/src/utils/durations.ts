// Convert a duration (double) measured in seconds to (hh:mm:)ss.ddd
export const formatTime = (seconds: number | undefined): string => {

  const rSeconds = seconds ? Math.round(seconds * 1000) / 1000 : 0
  
  let dur = ''
  
  const hours = Math.floor(rSeconds / 3600)
  if (hours) dur += hours + ':'

  const minutes = Math.floor((rSeconds / 60) % 60)
  if (minutes || hours) dur += String(minutes).padStart(2, '0') + ':'
  
  
  const iSeconds = Math.floor(rSeconds % 60)
  dur += String(iSeconds).padStart(2, '0')
  
  const milliseconds = Math.round((rSeconds % 1) * 1000)
  dur += '.' + String(milliseconds).padEnd(3, '0')

  return dur
}