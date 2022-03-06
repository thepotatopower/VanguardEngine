-- 彼方を目指して ピアエル

function RegisterAbilities()
	-- cont1
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.BlackWings)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
end

function Cont()
	if obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Alestiel")}) then
		obj.AddCardState({q.Other, o.This}, cs.CanAttackGrade3OrGreaterVanguardFromBackRow, p.Continuous)
	end
end