-- Steam Battler, Gungnram

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 3
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
			return true
		end
	elseif n == 2 then 
		if obj.IsRearguard() and not obj.Activated() and obj.CanSB(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
	elseif n == 2 then
		obj.Draw(1)
	end
	return 0
end