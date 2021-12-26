-- 封焔の巫女 バヴサーガラ

function RegisterAbilities()
	-- on armed
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnArmed)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("OnArmedTrigger")
	ability1.SetActivation("OnArmed")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCondition("ACTCondition")
	ability2.SetCost("ACTCost")
	ability2.SetActivation("ACT")
end

function OnArmedTrigger()
	return obj.IsApplicable()
end

function OnArmed()
	obj.SoulCharge(1)
	obj.SuperiorCall({q.Location, l.Drop, q.Grade, 1, q.Other, o.GradeOrLess, q.Count, 1})
end

function ACTCondition()
	return obj.GetNumberOf({q.Location, l.MyArms}) >= 2 
end

function ACTCost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function ACT()
	obj.Retire({q.Location, l.FrontRowEnemyRC})
	obj.AddCardValue({q.Location, l.PlayerVC, q.Other, o.This}, cs.BonusCritical, 1, p.UntilEndOfTurn)
end