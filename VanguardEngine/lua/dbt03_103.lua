-- 天躍の剣士 サリーレ

function RegisterAbilities()
	-- on place / on discard
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanReveal({q.Location, l.Hand, q.Grade, 3, q.Count, 3}) end
	obj.SoulBlast(1)
	obj.ChooseReveal({q.Location, l.Hand, q.Grade, 3, q.Count, 3})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Grade, 3, q.Count, 1})
end

function Activation()
	obj.ChooseSendToTop({q.Location, l.Drop, q.Grade, 3, q.Count, 1})
end