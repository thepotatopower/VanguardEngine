-- 極光戦姫 ティザー・ラージュ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetNotMovedFrom(l.Hand)
	ability1.SetLocation(l.FrontRowRC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
	ability1.SetProperty(p.OncePerTurn)
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Enemy, q.Other, o.SameZone})
end

function Activation()
	obj.Rest({q.Location, l.Applicable, q.Other, o.Enemy, q.Other, o.SameZone})
end