-- Time-fissuring Fist Colossus

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		if obj.CanCB(1) and obj.InFinalRush() and obj.YesNo("Counter Blast 1 to add 15000 power?") then
			obj.CounterBlast(1)
			obj.AddTempPower(1, 15000)
		end
	end
	return 0
end