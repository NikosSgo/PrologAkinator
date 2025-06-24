import "./globals.css";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={'bg-blue-400 min-h-screen p-[80px] pb-0'}
      >
        {children}
      </body>
    </html>
  );
}
