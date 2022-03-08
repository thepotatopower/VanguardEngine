-- 味わい舌鼓 ティグル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Powerful)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.SoulCount() == 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end