namespace StartIdea.UI.Models
{
    public class ChartDatasetFacade
    {
        public ChartDatasetFacade(string color)
        {
            lineTension = 0;
            fill = false;
            pointBorderWidth = 1;
            backgroundColor = color;
            borderColor = color;
            pointBorderColor = color;
            pointBackgroundColor = color;
            borderDash = new short[] { 0 };
        }

        public string label { get; set; }
        public double[] data { get; set; }
        public short lineTension { get; set; }
        public bool fill { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string pointBorderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public short pointBorderWidth { get; set; }
        public short[] borderDash { get; set; }
    }
}