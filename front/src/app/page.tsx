'use client'

import { useRouter } from 'next/navigation'

export default function Home() {

  const router = useRouter()

  const startGame = () => {
    router.push('/akinator')
  }

  return (
    <div className="bg-blue-300 rounded-4xl p-[50px] flex flex-col gap-10 text-white">

      <div className="text-center text-4xl font-extrabold bg-blue-600 p-2 rounded-2xl">Акинатор</div>

      <div className="bg-blue-600 h-[500px] rounded-2xl p-5 flex flex-col justify-around">

        <div className="text-center text-4xl">Задумай футболиста и я его отгадаю!</div>

        <div className="flex flex-row justify-center">
          <button 
            className="bg-yellow-300 text-black 
            px-[100px] py-2 rounded-md border-black border-solid border-2 font-medium
          hover:bg-yellow-400 transition duration-300 ease-in-out"
            onClick={startGame}
          >
            Давай начнём!
          </button>
        </div>

      </div>
    </div>
  );
}
