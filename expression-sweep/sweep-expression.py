# MIT LICENSE 
# 
# Script to sweep a expression and export step files
# please adjust the base path where to output the files

import os
import NXOpen

def main():


    the_session = NXOpen.Session.GetSession()
    work_part = the_session.Parts.Work


    listing = the_session.ListingWindow
    listing.Open()
    listing.WriteLine("Starting script")

    base_path = r"C:\Users\path\to\export\step\files\to"
    expression_name = "radius"
    sweep_values = list(range(10, 55, 5))

    listing.WriteLine(f"Exporting to: {base_path}")

    
    listing.WriteLine(f"Values List: {sweep_values}")

    for value in sweep_values:
        try:
            listing.WriteLine(f"Processing radius {value}")
            expr = work_part.Expressions.FindObject(expression_name)
            expr.Value = float(value)

            # Update the model after changing the expression
            mark = the_session.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update for value change")
            the_session.UpdateManager.DoUpdate(mark)

            # STEP file output
            
            if not os.path.exists(base_path):
                os.makedirs(base_path)
                
            step_filename = f"export_{value}.stp"
            step_file = os.path.join(base_path, step_filename)


            stepCreator = the_session.DexManager.CreateStepCreator()
            stepCreator.ExportAs = NXOpen.StepCreator.ExportAsOption.Ap214
            stepCreator.SettingsFile = "C:\\Program Files\\Siemens\\NX2212\\step214ug\\ugstep214.def"
            stepCreato = the_session.DexManager.CreateStepCreator()
            stepCreator.ExportAs = NXOpen.StepCreator.ExportAsOption.Ap214
            stepCreator.ExportSelectionBlock.SelectionScope = NXOpen.ObjectSelector.Scope.EntirePart
            
            stepCreator.OutputFile = step_file
    
            stepCreator.FileSaveFlag = False
            stepCreator.LayerMask = "1-256"
            stepCreator.ProcessHoldFlag = True
            
            stepCreator.Commit()
            stepCreator.Destroy()

            listing.WriteLine(f"Exported: {step_filename}")

        except Exception as e:
            listing.WriteLine(f"Failed at sweep step {value}: {str(e)}")

if __name__ == "__main__":
    main()

