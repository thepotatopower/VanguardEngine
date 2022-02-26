-- 極光戦姫 トレース・ジューヌ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation() 
	if obj.Exists({q.Location, l.PlayerPrisoners}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
		obj.AddCardState({q.Other, o.This}, cs.CanInterceptFromBackRow, p.Continuous)
	end
end