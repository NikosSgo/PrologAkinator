'use client'

import { useRouter } from 'next/navigation'

export default function FinalPage() {
  const router = useRouter()

  const startPage = () => {
    router.push('/')
  }

  const addPlayer = () => {
    router.push('/addPlayer')
  }

  return (
    <div className="bg-blue-300 rounded-4xl p-[50px] flex flex-col gap-10 text-white">
      <div className="text-center text-4xl font-extrabold bg-blue-600 p-2 rounded-2xl">
        Акинатор
      </div>

      <div className="bg-blue-600 h-[500px] rounded-2xl p-5 flex flex-col justify-around">
        <div className="text-center text-4xl">Твой персонаж — это:</div>
        <div className="text-center text-4xl">Лионеля Пепси!</div>

        <div className="flex justify-center gap-6">
          <button
            className="w-[250px] bg-green-400 text-black py-3 rounded-md border-2 border-black font-medium
            hover:bg-green-600 transition duration-300 ease-in-out"
            onClick={startPage}
          >
            Ты угадал!<br />Начать заново
          </button>

          <button
            className="w-[250px] bg-red-500 text-black py-3 rounded-md border-2 border-black font-medium
            hover:bg-red-700 transition duration-300 ease-in-out"
            onClick={addPlayer}
          >
            Ты не угадал!<br />Добавить своего футболиста
          </button>
        </div>
      </div>
    </div>
  )
}
