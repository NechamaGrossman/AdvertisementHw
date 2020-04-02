using AdvertisementHW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace AdvertisementModel
{
    public class ListingManager
    {
        public String _connectionString;

        public ListingManager()
        {
        }

        public ListingManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Item> GetAllItems()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select* From Item";
            conn.Open();
            List<Item> items = new List<Item>();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                Item i = new Item
                {
                    Name = (string)reader["Name"],
                    Text = (string)reader["Text"],
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber= (string)reader["PhoneNumber"]
  

                };
                items.Add(i);
            }
            return items;
        }
        public int AddItem(Item i)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Insert into Item (Name, Date, Text, PhoneNumber) Values (@Name, @Date, @Text, @PhoneNumber) ";
            cmd.Parameters.AddWithValue("@Name", i.Name);
            cmd.Parameters.AddWithValue("@Date", i.Date);
            cmd.Parameters.AddWithValue("@Text", i.Text);
            cmd.Parameters.AddWithValue("@PhoneNumber", i.PhoneNumber);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return (GetId(i));
        }
        public int GetId(Item i )
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select id from Item i where i.Name = @Name And i.Date =  @Date And i.Text= @Text";
            cmd.Parameters.AddWithValue("@Name", i.Name);
            cmd.Parameters.AddWithValue("@Date", i.Date);
            cmd.Parameters.AddWithValue("@Text", i.Text);
            conn.Open();
            int Id= new int();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                Id = (int)reader["Id"];
            }
            return Id;
        }
    }
}
