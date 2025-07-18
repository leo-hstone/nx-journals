' NX 12.0.2.9
' Journal created by Leo on Sat Nov  7 00:40:50 2020 W. Europe Standard Time
'
' MIT License
' 
' This script will set the folder where it is placed into as root folder for the assembly load options.
' Furthermore it will try to load all parts with the specified Reference Set order.
' 
'

Imports System
Imports NXOpen

Module NXJournal
Sub Main (ByVal args() As String) 

Dim theSession As NXOpen.Session = NXOpen.Session.GetSession()

' ----------------------------------------------
' Get Journal Path = Mockup Folder
' ----------------------------------------------

Dim journalPath = theSession.ExecutingJournal
Dim mockupFolder As System.IO.DirectoryInfo
mockupFolder = System.IO.Directory.GetParent(journalPath)

' ----------------------------------------------

theSession.Parts.LoadOptions.LoadLatest = False

theSession.Parts.LoadOptions.ComponentLoadMethod = NXOpen.LoadOptions.LoadMethod.SearchDirectories
Dim searchDirectories1(0) As String

' Set the parent of the journal dir as search directory
searchDirectories1(0) = mockupFolder.FullName
Dim searchSubDirs1(0) As Boolean

' activate to search in subdirs, this is equivalent to appending /... to the directory name
searchSubDirs1(0) = True
theSession.Parts.LoadOptions.SetSearchDirectories(searchDirectories1, searchSubDirs1)

' Load all components 
theSession.Parts.LoadOptions.ComponentsToLoad = NXOpen.LoadOptions.LoadComponents.All
' Partially load to enhance performance
theSession.Parts.LoadOptions.PartLoadOption = NXOpen.LoadOptions.LoadOption.PartiallyLoad
theSession.Parts.LoadOptions.SetInterpartData(False, NXOpen.LoadOptions.Parent.Partial)


theSession.Parts.LoadOptions.AllowSubstitution = False
theSession.Parts.LoadOptions.GenerateMissingPartFamilyMembers = True
theSession.Parts.LoadOptions.AbortOnFailure = False


theSession.Parts.LoadOptions.OptionUpdateSubsetOnLoad = NXOpen.LoadOptions.UpdateSubsetOnLoad.None


' This defines the reference set priority order. Default here is "as saved"
Dim refSets(3) As String
refSets(0) = "FINAL_PART_MOCKUP"
refSets(1) = "Use Model"
refSets(2) = "Entire Part"
refSets(3) = "As Saved"
theSession.Parts.LoadOptions.SetDefaultReferenceSets(refSets)
theSession.Parts.LoadOptions.ReferenceSetOverride = True

theSession.Parts.LoadOptions.SetBookmarkComponentsToLoad(True, False, NXOpen.LoadOptions.BookmarkComponents.LoadVisible)
theSession.Parts.LoadOptions.BookmarkRefsetLoadBehavior = NXOpen.LoadOptions.BookmarkRefsets.ImportData

' ----------------------------------------------

End Sub
End Module
