-- デザイアデビル ブベツー

function RegisterAbilities()
	-- on put
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnPut)
	ability1.SetMovedFrom(l.RC)
	ability1.SetMovedTo(l.Soul)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("OnPutTrigger")
	ability1.SetCost("OnPutCost")
	ability1.SetCanFullyResolve("OnPutCanFullyResolve")
	ability1.SetActivation("OnPut")
end

function OnPutTrigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player})
end

function OnPutCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnPutCanFullyResolve()
	return obj.CanSuperiorCallToSpecificCircle({q.Location, l.Hand, q.Count, 1}, FL.OpenCircle) or obj.SoulCount() >= 10 
end

function OnPut()
	obj.SuperiorCallToSpecificCircle({q.Location, l.Hand, q.Count, 1}, FL.OpenCircle)
	if obj.SoulCount() >= 10 then
		obj.Draw(1)
	end
end