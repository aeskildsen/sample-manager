import { useState, useEffect } from 'react'

export function useFetch<T>(url: string | null) {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    let cancelled = false;
    async function fetchData(url: string) {
      if (!url) return  
      
      try {
        setLoading(true);
        const response = await fetch(url)

        if (!response.ok) {
          throw new Error(`HTTP error - status ${response.status}`)
        }

        const json = await response.json()
        // only set data if we're still supposed to
        if (!cancelled) {
          setData(json)
        }
      } catch(e) {
        setError(e as Error)
      } finally {
        setLoading(false)
      }
    }

    fetchData(url)

    // cleanup
    return () => {
      cancelled = true;
    }
  }, [url])

  return { data, loading, error }
}