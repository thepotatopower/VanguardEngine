-- 柩機の竜 アブストリム

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsAbyssalDarkNight() then
		obj.AddCardState({q.Location, l.PlayerRC, q.Other, o.This}, cs.CanAttackBackRow, p.Continuous)
	end
end