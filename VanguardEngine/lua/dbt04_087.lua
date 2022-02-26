-- 柩機の兵 ネグリタ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation() 
	local tokens = {q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token}
	obj.AddCardState(tokens, cs.Resist, p.Continuous)
	obj.AddCardState(tokens, cs.CannotBeRetiredByCardEffect, p.Continuous)
end