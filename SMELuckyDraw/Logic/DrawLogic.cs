using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SMELuckyDraw.Model;
using SMELuckyDraw.Util;
using System.Data;
using System.IO;

namespace SMELuckyDraw.Logic
{
    public class DrawLogic
    {
        private Dictionary<int, Candidate> _candidateList = new Dictionary<int, Candidate>();
        private Dictionary<int, Candidate> _exceptionList = new Dictionary<int, Candidate>();
        private int _maxCount = 0;
        private static DrawLogic _instance = new DrawLogic();
        private bool isInitialized = false;
        private DrawLogic() { }

        public static DrawLogic Instance()
        {
            return _instance;
        }

        public void Init()
        {
            if (isInitialized)
            {
                return;
            }

            try
            {
                prepareCandidateList();
                prepareExceptionList();
                isInitialized = true;
            }
            catch (Exception e)
            {
                LogHelper.DEBUG("Init logic failed.", e);
                throw;
            }
        }

        private void prepareCandidateList()
        {
            LogHelper.DEBUG("prepareCandidateList");
            //STEP 1, read from excel to DataTable
            string currDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string excelName = ConfigHelper.Instance().GetAppSettings("excelName");
            string excelPath = Path.Combine(currDir, "excel");
            excelPath = Path.Combine(excelPath, excelName);
            DataTable dtCandidates = ExcelHelper.ExcelToDatatable(excelPath, true, false);

            //STEP2, loop DataTable, put each candidate to list
            if (dtCandidates != null)
            {
                int idx = 0;
                foreach (DataRow row in dtCandidates.Rows)
                {
                    Candidate cdt = new Candidate();
                    cdt.Id = row[0].ToString();
                    cdt.Name = row[1].ToString();
                    _candidateList.Add(idx++, cdt);
                }
                _maxCount = idx;
            }
        }

        private void prepareExceptionList()
        {
            string strExcp = ConfigHelper.Instance().GetAppSettings("exceptionList");
            strExcp.TrimEnd();
            strExcp.TrimEnd(',');
            string[] listExcp = strExcp.Split(',');

            foreach (string str in listExcp)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }

                int id = Convert.ToInt32(str);
                _exceptionList.Add(id, _candidateList[id]);
            }
        }

        private int getNextNumberFromList()
        {
            int res = 0;
            if (_exceptionList.Count >= _candidateList.Count)
            {
                return -1;
            }

            do
            {
                Random random = new Random();
                res = random.Next(_maxCount);
            }
            while (_exceptionList.ContainsKey(res));

            return res;
        }

        public bool IsAbleToDraw()
        {
            return _candidateList.Count > 0 && _candidateList.Count > _exceptionList.Count;
        }

        public Candidate DoDraw()
        {
            int id = getNextNumberFromList();

            //id = -1, no candidate left
            if (id == -1)
            {
                return null;
            }

            string winner = _candidateList[id].Id + "  " + _candidateList[id].Name;
            _exceptionList.Add(id, _candidateList[id]);
            ConfigHelper.Instance().AppendAppSettings("exceptionList", id + ", ");
            ConfigHelper.Instance().AppendAppSettings("winnerList", winner + "  /  ");

            LogHelper.INFO(winner);

            return _candidateList[id];
        }

        public Candidate FreeDraw()
        {
            Random random = new Random();
            int idx = random.Next(_maxCount);
            return _candidateList[idx];
        }

        public int GetExceptionCount()
        {
            return _exceptionList.Count;
        }

        /// <summary>
        /// Reset app to init state
        /// </summary>
        public void ResetApp()
        {
            LogHelper.DEBUG("ResetApp");
            _exceptionList.Clear();
            ConfigHelper.Instance().UpdateAppSettings("exceptionList", "");
            ConfigHelper.Instance().UpdateAppSettings("winnerList", "");
        }
    }
}
