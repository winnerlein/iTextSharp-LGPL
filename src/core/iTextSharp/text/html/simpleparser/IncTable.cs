using System;
using System.Collections;
using iTextSharp4.text;
using iTextSharp4.text.pdf;
/*
 * Copyright 2004 Paulo Soares
 *
 * The contents of this file are subject to the Mozilla Public License Version 1.1
 * (the "License"); you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the License.
 *
 * The Original Code is 'iText, a free JAVA-PDF library'.
 *
 * The Initial Developer of the Original Code is Bruno Lowagie. Portions created by
 * the Initial Developer are Copyright (C) 1999, 2000, 2001, 2002 by Bruno Lowagie.
 * All Rights Reserved.
 * Co-Developer of the code is Paulo Soares. Portions created by the Co-Developer
 * are Copyright (C) 2000, 2001, 2002 by Paulo Soares. All Rights Reserved.
 *
 * Contributor(s): all the names of the contributors are added in the source code
 * where applicable.
 *
 * Alternatively, the contents of this file may be used under the terms of the
 * LGPL license (the "GNU LIBRARY GENERAL PUBLIC LICENSE"), in which case the
 * provisions of LGPL are applicable instead of those above.  If you wish to
 * allow use of your version of this file only under the terms of the LGPL
 * License and not to allow others to use your version of this file under
 * the MPL, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the LGPL.
 * If you do not delete the provisions above, a recipient may use your version
 * of this file under either the MPL or the GNU LIBRARY GENERAL PUBLIC LICENSE.
 *
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the MPL as stated above or under the terms of the GNU
 * Library General Public License as published by the Free Software Foundation;
 * either version 2 of the License, or any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU Library general Public License for more
 * details.
 *
 * If you didn't download this code from the following link, you should check if
 * you aren't using an obsolete version:
 * http://www.lowagie.com/iText/
 */

namespace iTextSharp4.text.html.simpleparser {
    using iTextSharp4.text.pdf;

    /**
    *
    * @author  psoares
    */
    public class IncTable {
        private Hashtable props = new Hashtable();
        private ArrayList rows = new ArrayList();
        private ArrayList cols;
        /** Creates a new instance of IncTable */
        public IncTable(Hashtable props) {
            foreach (DictionaryEntry dc in props)
                this.props[dc.Key] = dc.Value;
        }
        
        public void AddCol(PdfPCell cell) {
            if (cols == null)
                cols = new ArrayList();
            cols.Add(cell);
        }
        
        public void AddCols(ArrayList ncols) {
            if (cols == null)
                cols = new ArrayList(ncols);
            else
                cols.AddRange(ncols);
        }
        
        public void EndRow() {
            if (cols != null) {
                cols.Reverse();
                rows.Add(cols);
                cols = null;
            }
        }
        
        public ArrayList Rows {
            get {
                return rows;
            }
        }
        
        public PdfPTable BuildTable() {
            if (rows.Count == 0)
                return new PdfPTable(1);
            int ncol = 0;
            ArrayList c0 = (ArrayList)rows[0];
            for (int k = 0; k < c0.Count; ++k) {
                ncol += ((PdfPCell)c0[k]).Colspan;
            }
            PdfPTable table = new PdfPTable(ncol);
            String width = (String)props["width"];
            if (width == null)
                table.WidthPercentage = 100;
            else {
                if (width.EndsWith("%"))
                    table.WidthPercentage = float.Parse(width.Substring(0, width.Length - 1), System.Globalization.NumberFormatInfo.InvariantInfo);
                else {
                    table.TotalWidth = float.Parse(width, System.Globalization.NumberFormatInfo.InvariantInfo);
                    table.LockedWidth = true;
                }
            }
            for (int row = 0; row < rows.Count; ++row) {
                ArrayList col = (ArrayList)rows[row];
                for (int k = 0; k < col.Count; ++k) {
                    table.AddCell((PdfPCell)col[k]);
                }
            }
            return table;
        }
    }
}