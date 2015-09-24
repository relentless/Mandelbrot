open System
open System.Drawing

let xMinMandelbrotCoord = -2.5
let xMaxMandelbrotCoord = 1.0

let yMinMandelbrotCoord = -1.0
let yMaxMandelbrotCoord = 1.0

let xOutputPixels = 179.0 // console width in characters.  default seems to be 80.  Set to n-1
let yOutputPixels = 59.0 // console height in lines. Default seems to be 25.  Set to n-1

let maxIterations = 20 // iterations to try before giving up

// character to be printed at each pixel, depending on iteration count
let characters = [| " ";  "+"; "x"; "*"; "#"; "%"; "$"; "@" |] |> Array.rev

// colour of character to be printed at each pixel, depending on iteration count
let colours = [|
    ConsoleColor.Black; 
    ConsoleColor.White; 
    ConsoleColor.Yellow; 
    ConsoleColor.Gray; 
    ConsoleColor.Cyan; 
    ConsoleColor.Green; 
    ConsoleColor.Magenta; 
    ConsoleColor.Red; 
    ConsoleColor.Blue;  
    ConsoleColor.DarkYellow; 
    ConsoleColor.DarkGray; 
    ConsoleColor.DarkCyan; 
    ConsoleColor.DarkGreen; 
    ConsoleColor.DarkMagenta; 
    ConsoleColor.DarkRed; 
    ConsoleColor.DarkBlue; |] |> Array.rev

type Scale = {inputRange:float*float; outputRange:float*float}
let upto b a = (a,b)

// take a number from one range and scale it to a different range
let scale ranges number =
    let min (a,b) = a
    let max (a,b) = b
    let onePercent = (number - min ranges.inputRange) / (max ranges.inputRange - min ranges.inputRange)
    onePercent * (max ranges.outputRange - min ranges.outputRange) + min ranges.outputRange

let withinRadius radius (x, y) = x*x + y*y < radius*radius

// returns one of the outputs as relative to iteration against maxIterations
let selectOutput iteration (outputs:'a array) =
    let outputIndex = (float iteration) |> scale {inputRange = 0.0 |> upto (float maxIterations); outputRange = 0.0 |> upto (float outputs.Length - 1.0)} |> int
    outputs.[outputIndex]

[<EntryPoint>]
let main _ = 
    
    Console.SetWindowSize(int xOutputPixels + 1, int yOutputPixels + 1);

    for yPixel in 0.0..yOutputPixels do
        for xPixel in 0.0..xOutputPixels do

            let xScaled = xPixel |> scale {inputRange = 0.0 |> upto xOutputPixels; outputRange = xMinMandelbrotCoord |> upto xMaxMandelbrotCoord}
            let yScaled = yPixel |> scale {inputRange = 0.0 |> upto yOutputPixels; outputRange = yMinMandelbrotCoord |> upto yMaxMandelbrotCoord}

            let rec applyOperation iteration x y =
                if (x,y) |> withinRadius 2.0 && 
                   iteration < maxIterations
                then
                    let nextX = x*x - y*y + xScaled
                    let nextY = 2.0*x*y + yScaled
                    applyOperation (iteration + 1) nextX nextY
                else
                    Console.ForegroundColor <- selectOutput iteration colours
                    printf "%s" (selectOutput iteration characters)

            applyOperation 0 0.0 0.0
            
    System.Console.ReadLine() |> ignore
    0
