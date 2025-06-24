'use client'

import { useRouter } from 'next/navigation'
import { useState } from 'react'

export default function AddPage() {
  const router = useRouter()
  const [name, setName] = useState('')

  const handleSubmit = () => {
    if (name.trim()) {
      // Здесь можно сделать POST-запрос или сохранить во временное состояние
      console.log('Добавлен футболист:', name)
      router.push('/') // Возврат на главную
    } else {
      alert('Введите имя футболиста!')
    }
  }

  return (
    <div className="bg-blue-300 rounded-4xl p-[50px] flex flex-col gap-10 text-white">
      <div className="text-center text-4xl font-extrabold bg-blue-600 p-2 rounded-2xl">
        Акинатор
      </div>

      <div className="bg-blue-600 h-[500px] rounded-2xl p-5 flex flex-col justify-around">
        <div className="text-center text-3xl mb-4">Добавь своего футболиста</div>

        <input
          type="text"
          placeholder="Имя футболиста"
          className="text-white text-xl px-4 py-2 rounded-md outline-none border-2 border-black w-full max-w-md mx-auto"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />

        <div className="flex justify-center mt-4">
          <button
            className="bg-yellow-300 text-black w-[250px] py-3 rounded-md border-2 border-black font-medium
              hover:bg-yellow-400 transition duration-300 ease-in-out"
            onClick={handleSubmit}
          >
            Добавить футболиста
          </button>
        </div>
      </div>
    </div>
  )
}
