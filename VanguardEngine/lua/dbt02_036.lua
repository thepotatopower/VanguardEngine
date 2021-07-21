-- Supernatural Extraction

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanCB(1) and obj.Exists(2) then
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

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.Discard(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(3)
		if obj.SoulCount() >= 10 and obj.YesNo("Add card from soul to hand?") then
			obj.ChooseAddToHand(3)
		end
	end
	return 0
end