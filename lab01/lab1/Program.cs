namespace lab1
{
    public class Program//����� ��� ���������
    {
        public static void Main(string[] args)//������� �������
        {
            var builder = WebApplication.CreateBuilder(args);//��������� ������������� � ������������� ����������
            builder.Services.AddHttpLogging(o => { });//��������� ��������� �����������
            var app = builder.Build();//������ ����������


            app.MapGet("/", () => "��� ������ ASPA");
            //���������� ������� ��� �������
            //��� ������� �� ���� ����� ����� ���������� ������ "��� ������ ASPA".

            app.UseHttpLogging();//���� ����������� ��� ����������� �������� � ��������� ���� ��������

            app.Run();//��������� ����������
        }
    }
}