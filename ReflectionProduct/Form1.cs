using ReflectionProduct.IVehicleService;
using ReflectionProduct.VehicleModel;
using ReflectionProduct.VehicleService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionProduct
{
    public partial class Form1 : Form
    {
        protected CarService service = new CarService();
        private Button addButton = new Button();
        private Button editButton = new Button();
        private Button deleteButton = new Button();
        private TextBox[] carTextBox;
        private Label[] carLabel;
        private List<object> carList = new List<object>();
        private DataGridView grid = new DataGridView();

        Dictionary<string, List<object>> dictionary = new Dictionary<string, List<object>>();
        Panel fieldPanel = new Panel();
        Panel gridPanel = new Panel();
        DataGridView MainGrid = new DataGridView();
        PropertyInfo[] carProperties;
        Type carType;
        String[] carModel;

        public Form1()
        {
            InitializeComponent();

           

        }

        void  GetAllProduct()
        {
            
            List<Bind> productList = new List<Bind>();
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Product).IsAssignableFrom(t) &&
                t != typeof(Product))
                .ToList();
            
            
            foreach (Type item in types)
            {
                productList.Add(new Bind {
                    Name = item.Name
                });

            }

            var list = new BindingList<Bind>(productList);
            MainGrid.DataSource = list;
            MainGrid.Top = 20;
            MainGrid.Left = 50;
            MainGrid.Width =  150;
            MainGrid.Height = productList.Count * 50;

            MainGrid.Click += new EventHandler(Model_Action);

            this.Controls.Add(MainGrid);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            GetAllProduct();
            
        }

        protected void Model_Action(object sender, EventArgs e)
        {
            if(this.fieldPanel.Controls.Count > 0)
            {
                this.fieldPanel.Controls.Clear();
                

            }

            object obj = GetType();

            PropertyInfo[] carProperties = obj.GetType().GetProperties();

            var objectName = (Bind)MainGrid.SelectedRows[0].DataBoundItem;

            carLabel = new Label[carProperties.Length];
            carTextBox = new TextBox[carProperties.Length];
            

            for (int i = 0; i < carProperties.Length; i++)
            {

                    carTextBox[i] = new TextBox();
                    carLabel[i] = new Label();

                    carLabel[i].Text =  carProperties[i].Name;
                    carLabel[i].Top = i * 40;
                    //carLabel[i].Left = 220;

                    carTextBox[i].Top = i * 40;
                    carTextBox[i].Left = 100;

            }
            fieldPanel.Left = 340;
            fieldPanel.Location = new Point(250,10);
            fieldPanel.Size = new Size(carProperties.Length * 60 , carProperties.Length * 40 + 100);
            // Set the Borderstyle for the Panel to three-dimensional.
            fieldPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            addButton.Text = "Add";
            addButton.Top = carProperties.Length * 40;
            addButton.Left = 100;
            addButton.Click += new EventHandler(Add_Click);
            this.Controls.Add(addButton);

            this.Controls.Add(fieldPanel);
            fieldPanel.Controls.Add(addButton);
            for (int j = 0; j < carProperties.Length; j++)
            {
                fieldPanel.Controls.Add(carLabel[j]);
                fieldPanel.Controls.Add(carTextBox[j]);
            }
            //var list = new BindingList<object>(carList);

            var list = dictionary.Where(y => y.Key == objectName.Name).SelectMany(d => d.Value);
            grid.DataSource = list.ToList();
            grid.Top = 20;
            grid.Left = (carProperties.Length * 100 )+ 120;
            grid.Width = carProperties.Length *120;
            grid.Height = 200;

            this.Controls.Add(grid);
        }

        public object GetType()
        {
            var bind = (Bind)MainGrid.SelectedRows[0].DataBoundItem;

            Type types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Product).IsAssignableFrom(t) &&
                t != typeof(Product) && t.Name == bind.Name).FirstOrDefault();
            object obj = Activator.CreateInstance(types);

            return obj;
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            object obj = GetType();
            var objectName = (Bind)MainGrid.SelectedRows[0].DataBoundItem;

            carProperties = obj.GetType().GetProperties();

            int propPosition = 0;
            carModel = new String[carProperties.Length];
            
            foreach (Control control in this.fieldPanel.Controls)
            {
                if(control is TextBox)
                {
                    carModel[propPosition] = control.Text;
                    propPosition++;
                }
            }
            
            if (carModel[0] != "")
            {

                for (int i = 0; i < carProperties.Length; i++)
                {
                    Type propType = carProperties[i].PropertyType;
                    TypeCode code = Type.GetTypeCode(propType);
                    obj.GetType().GetProperty(carProperties[i].Name).SetValue(obj, Convert.ChangeType(carModel[i], code), null);
                }

                carList.Add(obj);
                //var listType = typeof(List<>);
                //var constructedListType = listType.MakeGenericType(obj.GetType());
                //var instance = Activator.CreateInstance(constructedListType);
                //dictionary.Add(objectName.Name, carList);
                

                dictionary = service.Add(obj, dictionary, carList, objectName.Name);
                //var list = new BindingList<object>(carList);

                var list = dictionary.Where(y => y.Key == objectName.Name).SelectMany( d => d.Value );
                //grid.DataSource = dictionary.ToLookup(p => p.Key == objectName.Name).ToList();
                grid.DataSource = list.ToList();
                grid.Top = 20;
                grid.Left = 500;
                grid.Width = this.Width / 2;
                grid.Height = 400;
                grid.MouseDoubleClick += new MouseEventHandler(GetItem_Click);

                for (int i = 0; i < carProperties.Length; i++)
                {
                    //if (i == 0) carTextBox[i].Enabled = true;
                    carTextBox[i].Clear();
                }

                this.Controls.Add(grid);
            }


        }


        protected void GetItem_Click(object sender, MouseEventArgs e)
        {
            object obj = GetType();

            obj = (object)grid.SelectedRows[0].DataBoundItem;
            int i = 0;
            foreach (var item in obj.GetType().GetProperties())
            {
                carTextBox[i].Text = obj.GetType().GetProperty(item.Name).GetValue(obj,null).ToString();
                i++;
            }

            
            //this.Controls.Add(carTextBox);
            addButton.Visible = false;
            editButton.Text = "Edit";
            editButton.Top = obj.GetType().GetProperties().Length * 40;
            editButton.Left = 100;
            editButton.Click += new EventHandler(Edit_Click);

            deleteButton.Text = "Delete";
            deleteButton.Top = obj.GetType().GetProperties().Length * 45;
            deleteButton.Left = 100;
            deleteButton.Click += new EventHandler(Delete_Click);

            editButton.Visible = true;
            deleteButton.Visible = true;
            this.Controls.Add(fieldPanel);
            fieldPanel.Controls.Add(editButton);
            fieldPanel.Controls.Add(deleteButton);
        }


        protected void Edit_Click(object sender, EventArgs e) 
        {
            int propPosition = 0;
            foreach (Control control in this.fieldPanel.Controls)
            {
                if (control is TextBox)
                {
                    carModel[propPosition] = control.Text;
                    propPosition++;
                }
            }

            var objectName = (Bind)MainGrid.SelectedRows[0].DataBoundItem;
            if (carModel[0] != "")
            {
                object car = GetType();

                for (int i = 0; i < car.GetType().GetProperties().Length; i++)
                {
                    Type propType = car.GetType().GetProperties()[i].PropertyType;
                    TypeCode code = Type.GetTypeCode(propType);
                    car.GetType().GetProperty(car.GetType().GetProperties()[i].Name).SetValue(car, Convert.ChangeType(carModel[i] ?? "", code), null);
                }
                var Id = car.GetType().GetProperty("Id").GetValue(car, null);
                dictionary = service.Edit(Convert.ToInt32(Id), dictionary, car, objectName.Name);

                var list = new BindingList<object>(carList);

                for (int i = 0; i < car.GetType().GetProperties().Length; i++)
                {
                    if (i == 0) carTextBox[i].Enabled = true;
                    carTextBox[i].Clear();
                }
                var listProduct = dictionary.Where(y => y.Key == objectName.Name).SelectMany(d => d.Value);
                //grid.DataSource = dictionary.ToLookup(p => p.Key == objectName.Name).ToList();
                grid.DataSource = listProduct.ToList();
                addButton.Visible = true;
                editButton.Visible = false;
                deleteButton.Visible = false;
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            int propPosition = 0;
            var objectName = (Bind)MainGrid.SelectedRows[0].DataBoundItem;
            foreach (Control control in this.fieldPanel.Controls)
            {
                if (control is TextBox)
                {
                    carModel[propPosition] = control.Text;
                    propPosition++;
                }
            }
            if (carModel[0] != "")
            {
                object car = GetType();

                for (int i = 0; i < car.GetType().GetProperties().Length; i++)
                {
                    Type propType = car.GetType().GetProperties()[i].PropertyType;
                    TypeCode code = Type.GetTypeCode(propType);
                    car.GetType().GetProperty(car.GetType().GetProperties()[i].Name).SetValue(car, Convert.ChangeType(carModel[i], code), null);
                }
                dictionary = service.Delete(carList, dictionary, car, objectName.Name);

                var list = new BindingList<object>(carList);

                grid.DataSource = list;

                for (int i = 0; i < car.GetType().GetProperties().Length; i++)
                {
                    if (i == 0) carTextBox[i].Enabled = true;
                    carTextBox[i].Clear();
                }
                var listProduct = dictionary.Where(y => y.Key == objectName.Name).SelectMany(d => d.Value);
                //grid.DataSource = dictionary.ToLookup(p => p.Key == objectName.Name).ToList();
                grid.DataSource = listProduct.ToList();
                addButton.Visible = true;
                editButton.Visible = false;
                deleteButton.Visible = false;

            }
        }
    }
}
