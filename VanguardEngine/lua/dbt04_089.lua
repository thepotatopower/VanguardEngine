-- 極光戦姫 リストレイント・ピアニィ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerPrisoners})
end

function Activation()
	local count = obj.GetNumberOf({q.Location, l.PlayerPrisoners})
	obj.SoulCharge(count)
end