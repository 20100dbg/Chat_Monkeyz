using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;


namespace Chat_Monkeyz
{

    public partial class wndFiles : Torbo.DockableForm
    {
        public bool activated = false;


        public wndFiles()
        {
            InitializeComponent();
        }


        private void FileTransfer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }


        public void RefreshList()
        {
            g_files.Rows.Clear();

            foreach (Peer p in Program.tabPeer)
            {
                foreach (sFile file in p.tabFile)
                {
                    AddFile(file, p);
                }
            }

            if (g_files.Rows.Count > 0)
                g_files.Rows[0].Selected = true;
        }


        public void AddFile(sFile file, Peer peer)
        {
            int idxRow = g_files.Rows.Add(new object[] { file.name, file.StringSize(), file.etat, ((file.send) ? "Send" : "Receive"), peer.Pseudo });
            g_files.Rows[idxRow].Tag = new FileItem { file = file, peer = peer };
        }


        public void RemoveFile(sFile file)
        {
            int idxRow = GetRowIndexBySFile(file);
            g_files.Rows.RemoveAt(idxRow);
        }


        private void b_accept_Click(object sender, EventArgs e)
        {
            FileItem fi = (FileItem)g_files.SelectedRows[0].Tag;
            fi.file.etat = FileStatus.Accepted;
            fi.peer.SendData(new DataFile { fileinfo = fi.file });

            g_files.SelectedRows[0].Cells["State"].Value = FileStatus.Accepted;
            g_files.SelectedRows[0].Tag = fi;
        }


        private void b_reject_Click(object sender, EventArgs e)
        {
            FileItem fi = (FileItem)g_files.SelectedRows[0].Tag;
            fi.file.etat = FileStatus.Rejected;
            fi.peer.SendData(new DataFile { fileinfo = fi.file });

            g_files.SelectedRows[0].Cells["State"].Value = FileStatus.Rejected;
            g_files.SelectedRows[0].Tag = fi;
        }


        private void b_clear_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in g_files.Rows)
            {
                FileStatus status = (FileStatus)row.Cells["State"].Value;
                if (status == FileStatus.Finished || status == FileStatus.Rejected)
                    g_files.Rows.Remove(row);
            }
        }


        private void g_files_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                g_files.Rows[e.RowIndex].Selected = true;
            
        }


        private int GetRowIndexBySFile(sFile file)
        {
            bool found = false;
            int idx = 0;

            for (int n = g_files.Rows.Count; idx < n && !found; idx++)
            {
                sFile tmpfile = (sFile)g_files.Rows[idx].Tag;
                if (tmpfile.hash == file.hash)
                    found = true;
            }

            return (found) ? --idx : -1;
        }

    }


    class FileItem
    {
        public Peer peer;
        public sFile file;
    }
}