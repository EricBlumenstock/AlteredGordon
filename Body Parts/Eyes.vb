﻿''' <summary>
''' Allows our fisherman to 'see'
''' </summary>
Public Class Eyes
    Public Event SeesAFish()

    ''' <summary>
    ''' Find the bobber
    ''' </summary>    
    Public Function LookForBobber() As Boolean

        ' Find the default 'non-fish' cursor
        Dim NoFishCursor As Win32.CURSORINFO = Win32.GetNoFishCursor
        Dim FishCursor As Win32.CURSORINFO = NoFishCursor

        ' Grab the client size
        Dim WoWRect As Rectangle = Win32.GetWowRectangle

        ' Calculate the area to scan
        Dim xPosMin As Long = WoWRect.Width / 4
        Dim xPosMax As Long = xPosMin * 3
        Dim yPosMin As Long = WoWRect.Height / 4
        Dim yPosMax As Long = yPosMin * 3

        Dim XPOSSTEP As Long = (xPosMax - xPosMin) / My.Settings.ScanningSteps
        Dim YPOSSTEP As Long = (yPosMax - yPosMin) / My.Settings.ScanningSteps
        Dim XOFFSET As Long = XPOSSTEP / My.Settings.ScanningRetries


        ' For each try we are doing...
        For tryAgain As Integer = 0 To My.Settings.ScanningRetries - 1

            ' Scan the x Values
            For xPos As Long = xPosMin + (XOFFSET * tryAgain) To xPosMax Step XPOSSTEP

                Application.DoEvents() 'ToDo - make this a multithreaded application

                ' And the y Values
                For yPos As Long = yPosMin To yPosMax Step YPOSSTEP

                    ' Move the mouse
                    Win32.MoveMouse(WoWRect.X + xPos, WoWRect.Y + yPos)

                    ' Sleep (give the OS a chance to change the cursor
                    Threading.Thread.Sleep(My.Settings.ScanningDelay)

                    ' Grab the current cursor
                    FishCursor = Win32.GetCurrentCursor

                    ' If it's different...
                    If FishCursor.hCursor <> NoFishCursor.hCursor OrElse _
                    FishCursor.flags <> NoFishCursor.flags Then

                        ' WE FOUND A FISH!!!
                        RaiseEvent SeesAFish()
                        Return True
                    End If
                Next
            Next
        Next tryAgain

        Return False
    End Function
End Class
