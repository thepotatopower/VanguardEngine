-- 素敵な出会いを ヘルティ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.GetNumberOf({q.Location, l.PlayerRC}) == 0 or obj.GetNumberOf({q.Location, l.PlayerRC}) >= 4 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 10000, p.Continuous)
	end
end