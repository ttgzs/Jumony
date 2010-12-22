﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ivony.Fluent;
using System.ComponentModel;

namespace Ivony.Html
{

  /// <summary>
  /// 提供协助操作 Attribute 的扩展方法
  /// </summary>
  public static class AttributeExtensions
  {

    /// <summary>
    /// 获取指定名称的属性对象
    /// </summary>
    /// <param name="element">元素</param>
    /// <param name="name">属性名</param>
    /// <returns>属性对象，如果没找到，则返回null</returns>
    /// <remarks>
    /// 为了性能考虑，如果有多个同名的属性，此方法只返回第一个
    /// </remarks>
    public static IHtmlAttribute Attribute( this IHtmlElement element, string name )
    {
      if ( element == null )
        throw new ArgumentNullException( "element" );

      var attributes = element.Attributes();

      /*
      //即使按字典检索，由于大部分元素属性都很少，也不见得有性能提升
      var dictionary = attributes as IDictionary<string, IHtmlAttribute>;

      if ( dictionary != null )
        return dictionary[name];
      */

      return element.Attributes().FirstOrDefault( a => a.Name.EqualsIgnoreCase( name ) );
    }




    /// <summary>
    /// 获取属性值，与 AttributeValue 属性不同，Value 方法在属性对象为null时不会抛出异常
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <returns>属性值，如果属性对象为null，则返回null</returns>
    public static string Value( this IHtmlAttribute attribute )
    {
      if ( attribute == null )
        return null;

      return attribute.AttributeValue;
    }


    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <param name="value">要设置的属性值</param>
    /// <returns>被设置的属性对象</returns>
    public static IHtmlAttribute Value( this IHtmlAttribute attribute, string value )
    {

      if ( attribute == null )
        throw new ArgumentNullException( "attribute" );

      attribute.AttributeValue = value;
      return attribute;
    }


    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <param name="oldValue">要被替换的字符串</param>
    /// <param name="newValue">用于替换的字符串</param>
    /// <returns>被设置的属性对象</returns>
    public static IHtmlAttribute Value( this IHtmlAttribute attribute, string oldValue, string newValue )
    {
      if ( attribute == null )
        throw new ArgumentNullException( "attribute" );

      if ( oldValue == null )
        throw new ArgumentNullException( "oldValue" );

      attribute.AttributeValue = attribute.AttributeValue.Replace( oldValue, newValue );
      return attribute;
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式对象</param>
    /// <param name="replacement">替换字符串</param>
    /// <returns></returns>
    public static IHtmlAttribute Value( this IHtmlAttribute attribute, Regex pattern, string replacement )
    {
      if ( attribute == null )
        throw new ArgumentNullException( "attribute" );

      if ( pattern == null )
        throw new ArgumentNullException( "pattern" );

      attribute.AttributeValue = pattern.Replace( attribute.AttributeValue, replacement );
      return attribute;
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式对象</param>
    /// <param name="evaluator">用于每一步替换的计算函数</param>
    /// <returns></returns>
    public static IHtmlAttribute Value( this IHtmlAttribute attribute, Regex pattern, MatchEvaluator evaluator )
    {
      if ( attribute == null )
        throw new ArgumentNullException( "attribute" );

      if ( evaluator == null )
        throw new ArgumentNullException( "evaluator" );

      attribute.AttributeValue = pattern.Replace( attribute.AttributeValue, evaluator );
      return attribute;
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式</param>
    /// <param name="evaluator">用于每一步替换的计算函数</param>
    /// <returns></returns>
    public static IHtmlAttribute Value( this IHtmlAttribute attribute, string pattern, MatchEvaluator evaluator )
    {
      if ( attribute == null )
        throw new ArgumentNullException( "attribute" );

      if ( evaluator == null )
        throw new ArgumentNullException( "evaluator" );

      attribute.AttributeValue = Regex.Replace( attribute.AttributeValue, pattern, evaluator );
      return attribute;
    }

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="attribute">属性对象</param>
    /// <param name="evaluator">用于替换属性值的计算函数</param>
    /// <returns></returns>
    public static IHtmlAttribute Value( this IHtmlAttribute attribute, Func<string, string> evaluator )
    {
      if ( attribute == null )
        throw new ArgumentNullException( "attribute" );

      if ( evaluator == null )
        throw new ArgumentNullException( "evaluator" );

      attribute.AttributeValue = evaluator( attribute.AttributeValue );
      return attribute;
    }


    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="element">要设置属性值的元素</param>
    /// <param name="attributeName">属性名</param>
    /// <returns>属性设置器</returns>
    public static AttributeValueSetter SetAttribute( this IHtmlElement element, string attributeName )
    {

      if ( element == null )
        throw new ArgumentNullException( "element" );

      return new AttributeValueSetter( element, attributeName );
    }


    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="element">要设置属性值的元素</param>
    /// <param name="attributeName">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns>设置了属性的元素</returns>
    public static IHtmlElement SetAttribute( this IHtmlElement element, string attributeName, string value )
    {

      if ( element == null )
        throw new ArgumentNullException( "element" );

      return new AttributeValueSetter( element, attributeName ).Value( value );
    }


    /// <summary>
    /// 属性设置器，提供Value等方法方便的设置属性值
    /// </summary>
    public class AttributeValueSetter
    {
      private readonly IHtmlElement _element;
      private readonly string _attributeName;

      private readonly IHtmlAttribute _attribute;


      internal IHtmlElement Element
      {
        get { return _element; }
      }

      internal IHtmlAttribute Attribute
      {
        get { return _attribute; }
      }

      internal string AttributeValue
      {
        get { return _attribute.Value(); }
      }


      internal AttributeValueSetter( IHtmlElement element, string attributeName )
      {
        if ( element == null )
          throw new ArgumentNullException( "element" );

        if ( attributeName == null )
          throw new ArgumentNullException( "attributeName" );

        _element = element;
        _attributeName = attributeName;


        _attribute = _element.Attribute( attributeName );
        if ( _attribute == null )
          _attribute = _element.AddAttribute( attributeName );
      }

      /// <summary>
      /// 将属性值设置为空
      /// </summary>
      /// <returns>设置属性值的元素</returns>
      public IHtmlElement Null()
      {
        _attribute.AttributeValue = null;
        return _element;
      }

      /// <summary>
      /// 将属性值设置为指定字符串
      /// </summary>
      /// <param name="value">要设置的属性值</param>
      /// <returns>设置属性值的元素</returns>
      public IHtmlElement Value( string value )
      {
        _attribute.AttributeValue = value;
        return _element;
      }


      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="oldValue">要被替换的字符串</param>
      /// <param name="newValue">用于替换的字符串</param>
      /// <returns>被设置的属性对象</returns>
      public IHtmlElement Value( string oldValue, string newValue )
      {
        if ( oldValue == null )
          throw new ArgumentNullException( "oldValue" );


        _attribute.AttributeValue = _attribute.AttributeValue.Replace( oldValue, newValue );
        return _element;
      }


      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式对象</param>
      /// <param name="replacement">替换字符串</param>
      /// <returns></returns>
      public IHtmlElement Value( Regex pattern, string replacement )
      {
        if ( pattern == null )
          throw new ArgumentNullException( "pattern" );


        _attribute.AttributeValue = pattern.Replace( _attribute.AttributeValue, replacement );
        return _element;
      }


      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式对象</param>
      /// <param name="evaluator">用于每一步替换的计算函数</param>
      /// <returns></returns>
      public IHtmlElement Value( Regex pattern, MatchEvaluator evaluator )
      {
        if ( pattern == null )
          throw new ArgumentNullException( "pattern" );


        _attribute.AttributeValue = pattern.Replace( _attribute.AttributeValue, evaluator );
        return _element;
      }


      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式</param>
      /// <param name="evaluator">用于每一步替换的计算函数</param>
      /// <returns></returns>
      public IHtmlElement Value( string pattern, MatchEvaluator evaluator )
      {
        if ( pattern == null )
          throw new ArgumentNullException( "pattern" );

        if ( evaluator == null )
          throw new ArgumentNullException( "evaluator" );


        _attribute.AttributeValue = Regex.Replace( _attribute.AttributeValue, pattern, evaluator );
        return _element;
      }


      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="evaluator">用于替换属性值的计算函数</param>
      /// <returns></returns>
      public IHtmlElement Value( Func<string, string> evaluator )
      {
        if ( evaluator == null )
          throw new ArgumentNullException( "evaluator" );


        _attribute.AttributeValue = evaluator( _attribute.AttributeValue );
        return _element;
      }


      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="evaluator">用于替换属性值的计算函数</param>
      /// <returns></returns>
      public IHtmlElement Value( Func<IHtmlElement, string, string> evaluator )
      {
        if ( evaluator == null )
          throw new ArgumentNullException( "evaluator" );

        _attribute.AttributeValue = evaluator( _element, _attribute.AttributeValue );
        return _element;
      }


      /// <summary>
      /// 删除这个属性
      /// </summary>
      /// <returns></returns>
      public IHtmlElement Remove()
      {
        _attribute.Remove();
        return _element;
      }




      /// <summary>
      /// 复制属性值
      /// </summary>
      /// <param name="element">要复制属性值的元素</param>
      /// <returns></returns>
      public IHtmlElement From( IHtmlElement element )
      {
        if ( element == null )
          throw new ArgumentNullException( "element" );

        _attribute.AttributeValue = element.Attribute( _attributeName ).Value();
        return _element;
      }
    }





    /// <summary>
    /// 批量设置属性值
    /// </summary>
    /// <param name="elements">要设置属性值的元素</param>
    /// <param name="attributeName">属性名</param>
    /// <returns>属性设置器</returns>
    public static AttributeSetValueSetter SetAttribute( this IEnumerable<IHtmlElement> elements, string attributeName )
    {
      return new AttributeSetValueSetter( elements, attributeName );
    }


    /// <summary>
    /// 批量设置属性值
    /// </summary>
    /// <param name="elements">要设置属性值的元素</param>
    /// <param name="attributeName">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns>设置了属性的元素</returns>
    public static IEnumerable<IHtmlElement> SetAttribute( this IEnumerable<IHtmlElement> elements, string attributeName, string value )
    {
      elements.ForAll( e => e.SetAttribute( attributeName, value ) );

      return elements;
    }


    /// <summary>
    /// 批量属性设置器，提供Value方法方便的批量设置属性值
    /// </summary>
    public class AttributeSetValueSetter
    {
      private readonly IEnumerable<IHtmlElement> _elements;
      private readonly AttributeValueSetter[] _setters;
      //      private readonly string _attributeName;


      internal AttributeSetValueSetter( IEnumerable<IHtmlElement> elements, string attributeName )
      {
        if ( attributeName == null )
          throw new ArgumentNullException( "attributeName" );

        _elements = elements;
        //        _attributeName = attributeName;

        _setters = _elements.Select( e => e.SetAttribute( attributeName ) ).ToArray();
      }

      /// <summary>
      /// 将属性值设置为空
      /// </summary>
      /// <returns>设置属性值的元素</returns>
      public IEnumerable<IHtmlElement> Null()
      {
        _setters.ForAll( s => s.Null() );
        return _elements;
      }

      /// <summary>
      /// 将属性值设置为指定字符串
      /// </summary>
      /// <param name="value">要设置的属性值</param>
      /// <returns>设置属性值的元素</returns>
      public IEnumerable<IHtmlElement> Value( string value )
      {
        _setters.ForAll( s => s.Value( value ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="oldValue">要被替换的字符串</param>
      /// <param name="newValue">用于替换的字符串</param>
      /// <returns>被设置的属性对象</returns>
      public IEnumerable<IHtmlElement> Value( string oldValue, string newValue )
      {
        _setters.ForAll( s => s.Value( oldValue, newValue ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式对象</param>
      /// <param name="replacement">替换字符串</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Value( Regex pattern, string replacement )
      {
        _setters.ForAll( s => s.Value( pattern, replacement ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式对象</param>
      /// <param name="evaluator">用于每一步替换的计算函数</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Value( Regex pattern, MatchEvaluator evaluator )
      {
        _setters.ForAll( s => s.Value( pattern, evaluator ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="pattern">用于在属性值中查找匹配字符串的正则表达式</param>
      /// <param name="evaluator">用于每一步替换的计算函数</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Value( string pattern, MatchEvaluator evaluator )
      {
        _setters.ForAll( s => s.Value( pattern, evaluator ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="evaluator">用于替换属性值的计算函数</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Value( Func<string, string> evaluator )
      {
        _setters.ForAll( s => s.Value( evaluator ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="evaluator">用于替换属性值的计算函数</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Value( Func<IHtmlElement, string, string> evaluator )
      {
        _setters.ForAll( s => s.Value( evaluator ) );
        return _elements;
      }

      /// <summary>
      /// 设置属性值
      /// </summary>
      /// <param name="evaluator">用于替换属性值的计算函数</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Value( Func<int, string, string> evaluator )
      {
        _setters.ForAll( ( s, i ) => s.Value( evaluator( i, s.AttributeValue ) ) );
        return _elements;
      }


      /// <summary>
      /// 删除这个属性
      /// </summary>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> Remove()
      {
        _setters.ForAll( ( s, i ) => s.Remove() );
        return _elements;
      }


      /// <summary>
      /// 复制属性值
      /// </summary>
      /// <param name="element">要复制属性值的元素</param>
      /// <returns></returns>
      public IEnumerable<IHtmlElement> From( IHtmlElement element )
      {
        _setters.ForAll( s => s.From( element ) );
        return _elements;
      }

    }

  }
}
