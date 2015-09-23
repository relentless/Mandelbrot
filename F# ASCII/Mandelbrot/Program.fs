open System
open System.Drawing

let xMinMandelbrotCoord = -2.5
let xMaxMandebrotCoord = 1.0

let yMinMandelbrotCoord = -1.0
let yMaxMandelbrotCoord = 1.0

let xOutputPixels = 179.0 // console width in characters.  default seems to be 80.  Set to n-1
let yOutputPixels = 59.0 // console height in lines. Default seems to be 25.  Set to n-1

let maxIterations = 20 // iterations to try before giving up

let scale inputMax outputMin outputMax number =
    let onePercent = number / inputMax
    onePercent * (outputMax - outputMin) + outputMin

type Range = {from: float; upto: float }

let scale' inputRange outputRange number =
    let onePercent = (number - inputRange.from ) / (inputRange.upto - inputRange.from)
    onePercent * (outputRange.upto - outputRange.from) + outputRange.from

let outputs = [| " "; "."; "+"; "o"; "x"; "*"; "#"; "%"; "$"; "@" |] |> Array.rev

let colours = [| ConsoleColor.Black; ConsoleColor.Yellow; ConsoleColor.Cyan; ConsoleColor.Red; ConsoleColor.DarkGreen |] |> Array.rev

let withinRadius radius (x, y) = x*x + y*y < radius*radius

[<EntryPoint>]
let main _ = 
    
    Console.SetWindowSize(int xOutputPixels + 1, int yOutputPixels + 1);
    Console.SetWindowPosition(0,0)

    for yPixel in 0.0..yOutputPixels do
        for xPixel in 0.0..xOutputPixels do

            let xScaled = xPixel |> scale' {from=0.0; upto=xOutputPixels} {from=xMinMandelbrotCoord; upto=xMaxMandebrotCoord} 
            let yScaled = yPixel |> scale yOutputPixels yMinMandelbrotCoord yMaxMandelbrotCoord

            let rec applyOperation iteration x y =
                if (x,y) |> withinRadius 2.0 && iteration < maxIterations then
                    let nextX = x*x - y*y + xScaled
                    let nextY = 2.0*x*y + yScaled
                    applyOperation (iteration + 1) nextX nextY
                else
                    let outputIndex = (float iteration) |> scale (float maxIterations) 0.0 (float outputs.Length - 1.0) |> int 
                    let outputString = outputs.[outputIndex]

                    let outputColourIndex = (float iteration) |> scale (float maxIterations) 0.0 (float colours.Length - 1.0) |> int 
                    let outputColour = colours.[outputColourIndex]
            
                    Console.ForegroundColor <- outputColour
                    printf "%s" outputString

            applyOperation 0 0.0 0.0
            
    System.Console.ReadLine() |> ignore
    0
