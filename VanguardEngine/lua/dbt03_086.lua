-- グラビディア・レーグル

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsAttackingUnit() and obj.Exists({q.Location, l.Order, q.Other, o.Meteorite, q.Count, 3}) then
		obj.AddPlayerValue(ps.GuardRestrict, 2, p.Continuous, false)
	end
end