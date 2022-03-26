-- 友誼の騎士 ケイ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt05_030"), q.Other, o.Attacking}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end