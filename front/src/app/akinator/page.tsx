'use client'

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'

type Answer = {
  question: string
  answer: string
}

type AkinatorResponse = {
  result?: string
  not_found?: string
  next_question?: string
}

export default function AkinatorPage() {
  const router = useRouter()
  const [question, setQuestion] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [answer, setAnswer] = useState<string | null>(null)
  const [answerList, setAnswerList] = useState<Answer[]>([])
  const [gameFinished, setGameFinished] = useState(false)

  useEffect(() => {
    async function fetchFirstQuestion() {
      setLoading(true)
      setError(null)
      try {
        const res = await fetch('http://localhost:5000/first_question')
        if (!res.ok) throw new Error('Ошибка при загрузке первого вопроса')
        const data = await res.json()
        setQuestion(data.question)
      } catch (e) {
        setError('Ошибка при загрузке первого вопроса')
        console.error(e)
      } finally {
        setLoading(false)
      }
    }
    fetchFirstQuestion()
  }, [])

  const sendAnswer = async () => {
    if (!question || (!answer && !gameFinished)) return

    if (gameFinished) {
      // Возвращаем на начальную страницу
      router.push('/')
      return
    }

    setLoading(true)
    setError(null)
    try {
      const answersToSend = [...answerList, { question, answer: answer! }]
      const res = await fetch('http://localhost:5000/akinator', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ answers: answersToSend }),
      })
      if (!res.ok) throw new Error('Ошибка при отправке ответа')
      const data: AkinatorResponse = await res.json()

      if (data.result) {
        setQuestion(`Это ${data.result}!`)
        setGameFinished(true)
      } else if (data.not_found) {
        setQuestion("Нет подходящих игроков")
        setGameFinished(true)
      } else if (data.next_question) {
        setQuestion(data.next_question)
        setAnswerList(answersToSend)
      } else {
        setQuestion('Вопросы закончились, результат не найден')
        setGameFinished(true)
      }
      setAnswer(null)
    } catch (e) {
      setError('Ошибка при отправке ответа')
      console.error(e)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="bg-blue-300 rounded-4xl p-[50px] flex flex-col gap-10 text-white max-w-3xl mx-auto">
      <div className="text-center text-4xl font-extrabold bg-blue-600 p-2 rounded-2xl">Акинатор</div>

      <div className="bg-blue-600 min-h-[500px] rounded-2xl p-5 flex flex-col justify-around items-center">
        {loading ? (
          <div className="text-center text-3xl">Загрузка...</div>
        ) : error ? (
          <div className="text-center text-red-400 text-xl">{error}</div>
        ) : (
          <>
            <div className="text-center text-4xl mb-8 px-4">{question ?? "Загрузка данных..."}</div>

            {!gameFinished && (
              <div className="flex gap-10">
                <button
                  className="bg-green-400 text-black px-16 py-4 rounded-xl font-semibold border-2 border-black hover:bg-green-500 transition"
                  onClick={() => setAnswer("yes")}
                >
                  Да
                </button>

                <button
                  className="bg-red-400 text-black px-16 py-4 rounded-xl font-semibold border-2 border-black hover:bg-red-500 transition"
                  onClick={() => setAnswer("no")}
                >
                  Нет
                </button>
              </div>
            )}
          </>
        )}

        <button
          className="mt-10 bg-yellow-300 text-black px-24 py-3 rounded-md border-2 border-black font-medium
          hover:bg-yellow-400 transition duration-300 ease-in-out disabled:opacity-50 disabled:cursor-not-allowed"
          onClick={sendAnswer}
          disabled={(!answer && !gameFinished) || question === null}
        >
          {gameFinished ? 'Готово!' : 'Следующий вопрос!'}
        </button>
      </div>
    </div>
  )
}