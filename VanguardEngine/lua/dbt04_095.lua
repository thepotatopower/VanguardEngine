-- 極光戦姫 ライオット・ビーブル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.Exists({q.Location, l.PlayerVC, q.Grade, 4}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 15000, p.Continuous)
	end
end