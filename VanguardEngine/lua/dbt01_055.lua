-- 霊体凝縮

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function GetCosts()
	return p.SB, 1
end

function Activation()
	obj.Store(obj.SuperiorCall({q.Location, l.Drop, q.Grade, obj.GetGrade({q.Location, l.PlayerVC}), q.Other, o.GradeOrLess, q.Count, 1, q.Min, 0}))
	obj.AddCardValue({q.Location, l.Stored}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end