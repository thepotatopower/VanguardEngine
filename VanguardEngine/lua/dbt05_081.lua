-- 輝かしい光の下で テグリア

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.WasRodeUponByNameContains("Thegrea")
end

function Activation()
	obj.RevealFromDeck(1)
	obj.SuperiorCallAsRest({q.Location, l.Revealed, q.Grade, 2, q.Other, o.GradeOrLess, q.Other, o.Unit, q.Count, 1, q.Min, 0})
end