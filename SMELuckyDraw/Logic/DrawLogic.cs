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
        private DrawLogic() { }

        public static DrawLogic Instance()
        {
            return _instance;
        }

        public void Init()
        {
            prepareCandidateList();
        }

        private void prepareCandidateList()
        {
            LogHelper.DEBUG("prepareCandidateList");
            //STEP 1, read from excel to DataTable
            string currDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string excelName = ConfigurationManager.AppSettings["excelName"];
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

            _exceptionList.Add(id, _candidateList[id]);

            return _candidateList[id];
        }

        public Candidate FreeDraw(int seed)
        {
            Random random = new Random(seed);
            int idx = random.Next(_maxCount);
            return _candidateList[idx];
        }

        /// <summary>
        /// Reset app to init state
        /// </summary>
        public void ResetApp()
        {
            LogHelper.DEBUG("ResetApp");
            _exceptionList.Clear();
        }        
    }
}
